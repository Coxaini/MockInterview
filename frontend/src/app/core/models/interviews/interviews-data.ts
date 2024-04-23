import { Interview } from './interview';
import { UpcomingInterview } from './upcoming-interview';

export interface InterviewsData {
    plannedInterviews: UpcomingInterview[];
    endedInterviews: Interview[];
}
