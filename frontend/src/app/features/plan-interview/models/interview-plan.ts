import { UserRole } from '@core/models/users/user-role';

export interface InterviewPlan {
    programmingLanguage: string;
    technologies: string[];
    role: UserRole;
    startTime: string;
}
