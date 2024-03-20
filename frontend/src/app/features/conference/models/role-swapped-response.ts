import { ConferenceMemberRole } from './conference-member-role';
import { ConferenceQuestion } from './conference-question';

export interface RoleSwappedResponse {
    interviewId: string;
    newRole: ConferenceMemberRole;
    currentQuestion?: ConferenceQuestion;
}
