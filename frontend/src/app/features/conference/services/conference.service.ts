import { Injectable, OnDestroy } from '@angular/core';
import { environment } from '@env/environment';
import * as signalR from '@microsoft/signalr';
import { ReplaySubject, Subject, from, switchMap } from 'rxjs';
import { UserConference } from '@features/conference/models/user-conference';
import { HttpClient } from '@angular/common/http';
import { UserSessionData } from '../models/user-joined-data';
import { RoleSwappedResponse } from '../models/role-swapped-response';
import { ChangeQuestion } from '../models/change-question';
import { ConferenceQuestion } from './../models/conference-question';

@Injectable()
export class ConferenceService implements OnDestroy {
    constructor(private httpClient: HttpClient) {}

    private hubConnection: signalR.HubConnection;

    private userJoined = new ReplaySubject<UserSessionData>(1);
    userJoined$ = this.userJoined.asObservable();

    private offerReceived = new Subject<RTCSessionDescriptionInit>();
    offerReceived$ = this.offerReceived.asObservable();

    private answerReceived = new Subject<RTCSessionDescriptionInit>();
    answerReceived$ = this.answerReceived.asObservable();

    private iceCandidateReceived = new Subject<RTCIceCandidateInit>();
    iceCandidateReceived$ = this.iceCandidateReceived.asObservable();

    private roleSwapped = new Subject<RoleSwappedResponse>();
    roleSwapped$ = this.roleSwapped.asObservable();

    private questionChanged = new Subject<ChangeQuestion>();
    questionChanged$ = this.questionChanged.asObservable();

    private userLeft = new Subject<UserSessionData>();
    userLeft$ = this.userLeft.asObservable();

    private conferenceEnded = new Subject<void>();
    conferenceEnded$ = this.conferenceEnded.asObservable();

    private joinConference(interviewId: string) {
        return this.hubConnection.invoke<UserConference>(
            'JoinConference',
            interviewId,
        );
    }

    public swapRoles(interviewId: string) {
        return this.hubConnection.invoke<RoleSwappedResponse>(
            'SwapRoles',
            interviewId,
        );
    }

    public endConference(interviewId: string) {
        return this.hubConnection.invoke('EndConference', interviewId);
    }

    public changeQuestion(interviewId: string, questionId: string) {
        return this.hubConnection.invoke<ChangeQuestion>(
            'ChangeQuestion',
            interviewId,
            questionId,
        );
    }

    public startConnection(interviewId: string) {
        this.hubConnection = new signalR.HubConnectionBuilder()
            .withUrl(`${environment.apiUrl}/conference-hub`)
            .build();

        const connection = this.hubConnection.start();

        this.hubConnection.on(
            'UserJoinedConference',
            (user: UserSessionData) => {
                console.log('User joined', user);
                this.userJoined.next(user);
            },
        );

        this.hubConnection.on('UserLeftConference', (user: UserSessionData) => {
            console.log('User left', user);
            this.userLeft.next(user);
        });

        this.hubConnection.on('ReceiveAnswer', (answer: string) => {
            console.log('Received answer', answer);
            this.answerReceived.next(
                JSON.parse(answer) as RTCSessionDescriptionInit,
            );
        });

        this.hubConnection.on('ReceiveOffer', (offer: string) => {
            console.log('Received offer', offer);
            this.offerReceived.next(
                JSON.parse(offer) as RTCSessionDescriptionInit,
            );
        });

        this.hubConnection.on('RoleSwapped', (swap: RoleSwappedResponse) => {
            this.roleSwapped.next(swap);
        });

        this.hubConnection.on(
            'QuestionChanged',
            (conferenceId: string, currentQuestion?: ConferenceQuestion) => {
                this.questionChanged.next({ conferenceId, currentQuestion });
            },
        );

        this.hubConnection.on('ConferenceEnded', () => {
            this.conferenceEnded.next();
        });

        this.subscribeToIceCandidate();

        return from(connection).pipe(
            switchMap(() => this.joinConference(interviewId)),
        );
    }

    public sendOffer(conferenceId: string, offer: RTCSessionDescriptionInit) {
        console.log('Sending offer', offer);
        this.hubConnection.invoke(
            'SendOffer',
            conferenceId,
            JSON.stringify(offer),
        );
    }

    public sendAnswer(conferenceId: string, answer: RTCSessionDescriptionInit) {
        console.log('Sending answer', answer);
        this.hubConnection.invoke(
            'SendAnswer',
            conferenceId,
            JSON.stringify(answer),
        );
    }

    private subscribeToIceCandidate() {
        this.hubConnection.on('ReceiveIceCandidate', (candidate: string) => {
            console.log('Received ice candidate', candidate);
            this.iceCandidateReceived.next(
                JSON.parse(candidate) as RTCIceCandidateInit,
            );
        });
    }

    public sendIceCandidate(conferenceId: string, candidate: RTCIceCandidate) {
        //console.log('Sending ice candidate', candidate);
        this.hubConnection.invoke(
            'SendIceCandidate',
            conferenceId,
            JSON.stringify(candidate.toJSON()),
        );
    }

    ngOnDestroy(): void {
        console.log('Conference service destroyed');
        this.hubConnection.stop();
    }
}
