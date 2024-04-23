import { differenceInMinutes } from 'date-fns';

export function getDuration(startDateTime: Date, endDateTime: Date) {
    const diffInMinutes = differenceInMinutes(endDateTime, startDateTime);
    const hours = Math.floor(diffInMinutes / 60);
    const minutes = diffInMinutes % 60;

    return (hours > 0 ? `${hours}h ` : '') + `${minutes}m`;
}
