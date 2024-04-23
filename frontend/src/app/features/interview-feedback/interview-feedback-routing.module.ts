import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InterviewFeedbackPageComponent } from './interview-feedback-page/interview-feedback-page.component';

const routes: Routes = [
    {
        path: ':interviewId',
        component: InterviewFeedbackPageComponent,
    },
    { path: '**', redirectTo: '/not-found' },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class InterviewFeedbackRoutingModule {}
