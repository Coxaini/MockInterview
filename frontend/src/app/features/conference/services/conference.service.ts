import { Injectable, OnDestroy } from '@angular/core';
import { environment } from '@env/environment';
import * as signalR from '@microsoft/signalr';
import { ReplaySubject, Subject, from } from 'rxjs';
import { UserConference } from '@features/conference/models/user-conference';
import { HttpClient } from '@angular/common/http';
import { UserSessionData } from '../models/user-joined-data';

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

    private userLeft = new Subject<UserSessionData>();
    userLeft$ = this.userLeft.asObservable();

    public joinConference(interviewId: string) {
        return this.httpClient.post<UserConference>(
            `conferences/${interviewId}/join`,
            {},
        );
    }

    public startConnection() {
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

        this.subscribeToIceCandidate();

        return from(connection);
    }

    // public subscribeToOffer() {
    //     this.hubConnection.on('ReceiveOffer', (offer: string) => {
    //         console.log('Received offer', offer);
    //         this.offerReceived.next(
    //             JSON.parse(offer) as RTCSessionDescriptionInit,
    //         );
    //     });

    //     return this.offerReceived.asObservable();
    // }

    public sendOffer(userId: string, offer: RTCSessionDescriptionInit) {
        console.log('Sending offer', offer);
        this.hubConnection.invoke('SendOffer', userId, JSON.stringify(offer));
    }

    // public subscribeToAnswer() {
    //     this.hubConnection.on('ReceiveAnswer', (answer: string) => {
    //         console.log('Received answer', answer);
    //         this.answerReceived.next(
    //             JSON.parse(answer) as RTCSessionDescriptionInit,
    //         );
    //     });

    //     return this.answerReceived.asObservable();
    // }

    public sendAnswer(userId: string, answer: RTCSessionDescriptionInit) {
        console.log('Sending answer', answer);
        this.hubConnection.invoke('SendAnswer', userId, JSON.stringify(answer));
    }

    private subscribeToIceCandidate() {
        this.hubConnection.on('ReceiveIceCandidate', (candidate: string) => {
            console.log('Received ice candidate', candidate);
            this.iceCandidateReceived.next(
                JSON.parse(candidate) as RTCIceCandidateInit,
            );
        });
    }

    public sendIceCandidate(userId: string, candidate: RTCIceCandidate) {
        console.log('Sending ice candidate', candidate);
        this.hubConnection.invoke(
            'SendIceCandidate',
            userId,
            JSON.stringify(candidate.toJSON()),
        );
    }

    ngOnDestroy(): void {
        console.log('Conference service destroyed');
        this.hubConnection.stop();
    }
}
