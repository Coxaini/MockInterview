import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { QuestionsListComponent } from './questions-list/questions-list.component';

const routes: Routes = [
    {
        path: ':id',
        component: QuestionsListComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class QuestionsRoutingModule {}
