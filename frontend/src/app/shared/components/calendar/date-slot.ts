import { DateTimeSlot } from '@shared/components/calendar/date-time-slot';

export class DateSlot {
    date: Date;
    timeSlots: DateTimeSlot[] = [];

    constructor(date: Date) {
        this.date = date;
    }
}
