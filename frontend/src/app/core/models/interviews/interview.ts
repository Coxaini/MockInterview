import { Mate } from './mate';

export interface Interview {
    id: string;
    mate: Mate;
    startDateTime: string;
    endDateTime: string;
    feedback?: string;
    programmingLanguage: string;
    tags: string[];
}
