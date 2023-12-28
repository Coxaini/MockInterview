import { RecommendationLevel } from '@features/plan-interview/models/recommendation-level';

export interface InterviewTimeSlot {
    startTime: string;
    recommendationLevel: RecommendationLevel;
}
