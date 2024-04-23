import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NotFoundPageComponent } from '@core/components/not-found-page/not-found-page.component';

const routes: Routes = [
    {
        path: 'auth',
        loadChildren: () =>
            import('./features/auth/auth.module').then((m) => m.AuthModule),
    },
    {
        path: 'fill-profile',
        loadChildren: () =>
            import('./features/fill-profile/fill-profile.module').then(
                (m) => m.FillProfileModule,
            ),
    },
    {
        path: 'conference',
        loadChildren: () =>
            import('./features/conference/conference.module').then(
                (m) => m.ConferenceModule,
            ),
    },
    {
        path: '',
        loadChildren: () =>
            import('./features/main/main.module').then((m) => m.MainModule),
    },
    {
        path: 'not-found',
        component: NotFoundPageComponent,
    },
    {
        path: '**',
        component: NotFoundPageComponent,
    },
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
})
export class AppRoutingModule {}
