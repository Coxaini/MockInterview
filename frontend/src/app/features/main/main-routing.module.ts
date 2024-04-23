import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainLayoutComponent } from '@core/components/layout/main-layout/main-layout.component';
import { HomePageComponent } from './home-page/home-page.component';

const routes: Routes = [
    {
        path: '',
        component: MainLayoutComponent,
        children: [
            {
                path: '',
                pathMatch: 'full',
                redirectTo: 'home',
            },
            {
                path: 'home',
                component: HomePageComponent,
            },
            {
                path: 'interview-feedback',
                loadChildren: () =>
                    import(
                        '../interview-feedback/interview-feedback.module'
                    ).then((m) => m.InterviewFeedbackModule),
            },
            {
                path: '',
                loadChildren: () =>
                    import('../interview/interview.module').then(
                        (m) => m.InterviewModule,
                    ),
            },
        ],
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class MainRoutingModule {}
