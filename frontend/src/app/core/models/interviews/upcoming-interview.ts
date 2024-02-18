import { Mate } from '@core/models/interviews/mate';

export interface UpcomingInterview {
    id: string;
    mate?: Mate;
    startDateTime: string;
    programmingLanguage: string;
    tags: string[];
}
