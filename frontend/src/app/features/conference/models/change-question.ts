import { ConferenceQuestion } from './conference-question';

export interface ChangeQuestion {
    conferenceId: string;
    currentQuestion?: ConferenceQuestion;
}
