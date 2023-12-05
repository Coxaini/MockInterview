import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FillProfilePageComponent } from './fill-profile-page/fill-profile-page.component';

const routes: Routes = [{ path: '', component: FillProfilePageComponent }];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class FillProfileRoutingModule {}
