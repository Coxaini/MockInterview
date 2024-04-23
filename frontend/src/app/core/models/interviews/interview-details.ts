import { Mate } from '@core/models/interviews/mate';
import { QuestionsList } from '@core/models/questions/questions-list';

export interface InterviewDetails {
    id: string;
    mate?: Mate;
    userQuestionsList: QuestionsList;
    mateQuestionsList?: QuestionsList;
    startDateTime: string;
    endDateTime?: string;
    programmingLanguage: string;
    tags: string[];
    type: 'arranged' | 'requested' | 'ended';
}
