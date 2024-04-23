import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { InterviewDetails } from '@core/models/interviews/interview-details';
import { InterviewService } from '@core/services/interviews/interview.service';
import {
    Observable,
    from,
    switchMap,
    shareReplay,
    map,
    iif,
    of,
    delayWhen,
    filter,
    tap,
    mergeMap,
    BehaviorSubject,
    startWith,
    Subject,
    forkJoin,
} from 'rxjs';
import { ConferenceService } from '../services/conference.service';
import { WebRtcService } from '../services/web-rtc.service';
import { UserConference } from '../models/user-conference';
import { ConferenceMemberRole } from '../models/conference-member-role';
import { UserRole } from '../models/user-role';
import { ConferenceQuestion } from '../models/conference-question';
import { ModalService } from '@core/services/modal/modal.service';

@Component({
    selector: 'app-conference-page',
    templateUrl: './conference-page.component.html',
    styleUrl: './conference-page.component.scss',
    providers: [ConferenceService, WebRtcService],
})
export class ConferencePageComponent implements OnInit, OnDestroy {
    localStream$: Observable<MediaStream>;
    remoteStream: MediaStream = new MediaStream();

    interview$: Observable<InterviewDetails>;
    conference$: Observable<UserConference>;

    isConnected$ = this.webRtcService.connected$;

    userRoleSubject = new Subject<UserRole>();

    currentQuestionSubject = new BehaviorSubject<ConferenceQuestion | null>(
        null,
    );

    isMediaSetupSubject = new BehaviorSubject<boolean>(false);

    constructor(
        private interviewService: InterviewService,
        private conferenceService: ConferenceService,
        private webRtcService: WebRtcService,
        private route: ActivatedRoute,
        private router: Router,
        private modalService: ModalService,
    ) {
        this.localStream$ = from(
            navigator.mediaDevices.getUserMedia({ video: true, audio: false }),
        ).pipe(shareReplay(1));
    }

    ngOnInit(): void {
        const interviewId$ = this.route.paramMap.pipe(
            map((params) => {
                const interviewId = params.get('interviewId');
                if (!interviewId) throw new Error('No interviewId');

                return interviewId;
            }),
            shareReplay({ bufferSize: 1, refCount: true }),
        );
        this.interview$ = interviewId$.pipe(
            tap((interviewId) => {
                console.log('InterviewId:', interviewId);
            }),
            switchMap((interviewId) => {
                return this.interviewService.getInterviewDetails(interviewId);
            }),
            shareReplay({ bufferSize: 1, refCount: true }),
        );

        const reconnected$ = this.conferenceService.userLeft$.pipe(
            switchMap(() => {
                return this.conferenceService.userJoined$;
            }),
            startWith(null),
        );

        this.conferenceService.userLeft$
            .pipe(
                filter((connection) => connection !== null),
                switchMap(() => {
                    return this.localStream$;
                }),
            )
            .subscribe((localStream) => {
                this.webRtcService.close();
                this.remoteStream = new MediaStream();

                this.webRtcService.setupMediaSources(
                    localStream,
                    this.remoteStream,
                );
            });

        this.conference$ = interviewId$.pipe(
            switchMap((conference) => {
                return this.isMediaSetupSubject.pipe(
                    filter((isSetup) => isSetup),
                    map(() => conference),
                );
            }),
            switchMap((interviewId) => {
                return this.conferenceService.startConnection(interviewId);
            }),
            switchMap((conference) => {
                return iif(
                    () => conference.isPeerJoined,
                    of(conference),
                    of({ ...conference, isPeerJoined: true }).pipe(
                        delayWhen(() => this.conferenceService.userJoined$),
                    ),
                );
            }),
            switchMap((conference) => {
                return reconnected$.pipe(map(() => conference));
            }),
            shareReplay(1),
        );

        this.conference$.subscribe((conference) => {
            this.userRoleSubject.next({ role: conference.userRole });
            this.currentQuestionSubject.next(
                conference.currentQuestion || null,
            );
        });

        this.localStream$.subscribe((localStream) => {
            this.webRtcService.setupMediaSources(
                localStream,
                this.remoteStream,
            );

            this.isMediaSetupSubject.next(true);
        });

        this.webRtcService.iceCandidateInitiated$
            .pipe(
                mergeMap((candidate) => {
                    return this.conference$.pipe(
                        map((conference) => {
                            return { conference, candidate };
                        }),
                    );
                }),
            )
            .subscribe(({ conference, candidate }) => {
                this.conferenceService.sendIceCandidate(
                    conference.id,
                    candidate,
                );
            });

        this.conferenceService.iceCandidateReceived$.subscribe((candidate) => {
            this.webRtcService.addIceCandidate(candidate);
        });

        this.initializeVideoConference();
        this.initializeConferenceSynchronization();
    }

    private initializeConferenceSynchronization() {
        this.conferenceService.roleSwapped$.subscribe((swappedInfo) => {
            this.userRoleSubject.next({ role: swappedInfo.newRole });
            console.log('Role swapped', swappedInfo);
            this.currentQuestionSubject.next(
                swappedInfo.currentQuestion || null,
            );
        });

        this.conferenceService.questionChanged$.subscribe((changeQuestion) => {
            console.log(changeQuestion);
            this.currentQuestionSubject.next(
                changeQuestion.currentQuestion || null,
            );
        });

        this.conferenceService.conferenceEnded$
            .pipe(switchMap(() => this.interview$))
            .subscribe((interview) => {
                this.router.navigate(['/interview-feedback', interview.id]);
            });
    }

    private initializeVideoConference() {
        this.conference$
            .pipe(
                filter((conference) => conference.shouldSendOffer),
                switchMap((conference) => {
                    return this.processOffer(conference);
                }),
            )
            .subscribe((answer) => {
                this.webRtcService.setRemoteDescription(answer);
            });

        this.conference$
            .pipe(
                filter((conference) => !conference.shouldSendOffer),
                switchMap((conference) => {
                    return this.processAnswer(conference);
                }),
            )
            .subscribe(({ conference, answer }) => {
                this.conferenceService.sendAnswer(conference.id, answer);
            });
    }

    private processOffer(conference: UserConference) {
        return from(this.webRtcService.createOffer()).pipe(
            switchMap((offer) => {
                this.conferenceService.sendOffer(conference.id, offer);
                return this.conferenceService.answerReceived$;
            }),
        );
    }

    private processAnswer(conference: UserConference) {
        return this.conferenceService.offerReceived$.pipe(
            switchMap((offer) => {
                return this.webRtcService.setRemoteDescription(offer);
            }),
            switchMap(() => {
                return this.webRtcService.createAnswer();
            }),
            map((answer) => {
                return { conference, answer };
            }),
        );
    }
    ngOnDestroy(): void {
        this.remoteStream.getTracks().forEach((track) => {
            track.stop();
        });
        if (this.isMediaSetupSubject.value) {
            this.localStream$.subscribe((stream) => {
                stream.getTracks().forEach((track) => {
                    track.stop();
                });
            });
        }
    }

    public isInterviewer(role: ConferenceMemberRole): boolean {
        return role === ConferenceMemberRole.Interviewer;
    }

    public swapRoles() {
        this.interview$
            .pipe(
                switchMap((interview) => {
                    return this.conferenceService.swapRoles(interview.id);
                }),
            )
            .subscribe({
                next: (role) => {
                    this.userRoleSubject.next({ role: role.newRole });
                    this.currentQuestionSubject.next(
                        role.currentQuestion || null,
                    );
                },
                error: (err) => {
                    console.error(err);
                },
            });
    }

    public endConference() {
        this.modalService
            .openConfirmationDialog(
                'End conference',
                'Are you sure you want to end the conference?',
            )
            .closed.pipe(
                switchMap((result) => {
                    if (!result) {
                        return of(null);
                    }
                    return this.interview$.pipe(
                        switchMap((interview) => {
                            return forkJoin([
                                of(interview),
                                this.conferenceService.endConference(
                                    interview.id,
                                ),
                            ]);
                        }),
                    );
                }),
            )
            .subscribe({
                next: (result) => {
                    if (!result) return;

                    const [interview] = result;

                    this.router.navigate(['/interview-feedback', interview.id]);
                },
                error: (err) => {
                    console.error(err);
                },
            });
    }

    public changeQuestion(questionId: string) {
        this.interview$
            .pipe(
                switchMap((interview) => {
                    return this.conferenceService.changeQuestion(
                        interview.id,
                        questionId,
                    );
                }),
            )
            .subscribe({
                next: (question) => {
                    this.currentQuestionSubject.next(
                        question.currentQuestion || null,
                    );
                },
                error: (err) => {
                    console.error(err);
                },
            });
    }
}
