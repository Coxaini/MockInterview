import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
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
} from 'rxjs';
import { ConferenceService } from '../services/conference.service';
import { WebRtcService } from '../services/web-rtc.service';
import { UserConference } from '../models/user-conference';

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

    isMediaSetupSubject = new BehaviorSubject<boolean>(false);

    constructor(
        private interviewService: InterviewService,
        private conferenceService: ConferenceService,
        private webRtcService: WebRtcService,
        private route: ActivatedRoute,
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
                return this.interviewService.getArrangedInterview(interviewId);
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
                return this.conferenceService
                    .startConnection()
                    .pipe(map(() => interviewId));
            }),
            switchMap((interviewId) => {
                return this.conferenceService.joinConference(interviewId);
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
                if (this.remoteStream.active) {
                    this.conferenceService.sendIceCandidate(
                        conference.peerId,
                        candidate,
                    );
                }
            });

        this.conferenceService.iceCandidateReceived$.subscribe((candidate) => {
            this.webRtcService.addIceCandidate(candidate);
        });

        this.joinVideoConference();
    }

    private joinVideoConference() {
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
                this.conferenceService.sendAnswer(conference.peerId, answer);
            });
    }

    private processOffer(conference: UserConference) {
        return from(this.webRtcService.createOffer()).pipe(
            switchMap((offer) => {
                this.conferenceService.sendOffer(conference.peerId, offer);
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
}
