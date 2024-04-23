import { Question } from './question';

export interface QuestionsList {
    id: string;
    interviewOrderId?: string;
    interviewId?: string;
    authorId: string;
    questions: Question[];
    feedback?: string;
}
