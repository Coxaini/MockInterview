import { QuestionsList } from '../questions/questions-list';

export interface InterviewOrderDetails {
    id: string;
    userQuestionsList: QuestionsList;
    programmingLanguage: string;
    startDateTime: string;
    tags: string[];
}
