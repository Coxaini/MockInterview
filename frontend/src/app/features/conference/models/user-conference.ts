import { ConferenceMemberRole } from './conference-member-role';
import { ConferenceQuestion } from './conference-question';

export interface UserConference {
    id: string;
    shouldSendOffer: boolean;
    peerId: string;
    isPeerJoined: boolean;
    userRole: ConferenceMemberRole;
    currentQuestion?: ConferenceQuestion;
}
