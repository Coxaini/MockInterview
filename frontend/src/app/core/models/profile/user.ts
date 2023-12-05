import { UserRole } from './user-role';

export interface User {
    id: string;
    email: string;
    name: string;
    bio: string;
    avatarUrl: string;
    yearsOfExperience: number;
    location: string;
    skills: string[];
    role: UserRole;
}
