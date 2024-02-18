import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InterviewPageComponent } from '@features/interview/interview-page/interview-page.component';
import { InterviewOrderPageComponent } from './interview-order-page/interview-order-page.component';

const routes: Routes = [
    {
        path: 'interviews/:interviewId',
        component: InterviewPageComponent,
    },
    {
        path: 'orders/:orderId',
        component: InterviewOrderPageComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class InterviewRoutingModule {}
