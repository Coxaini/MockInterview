import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ConferencePageComponent } from './conference-page/conference-page.component';

const routes: Routes = [
    {
        path: ':interviewId',
        component: ConferencePageComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ConferenceRoutingModule {}
