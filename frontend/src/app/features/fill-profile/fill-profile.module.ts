import { NgModule } from '@angular/core';
import { CommonModule, NgOptimizedImage } from '@angular/common';
import { FillProfileInfoComponent } from './fill-profile-info/fill-profile-info.component';
import { ReactiveFormsModule } from '@angular/forms';
import { WelcomeIntroComponent } from './welcome-intro/welcome-intro.component';
import { NgIconsModule } from '@ng-icons/core';
import { bootstrapChatLeftHeartFill } from '@ng-icons/bootstrap-icons';
import { FillProfileSkillsComponent } from './fill-profile-skills/fill-profile-skills.component';
import { RolesSelectComponent } from './roles-select/roles-select.component';
import { SelectableRoleCardComponent } from './selectable-role-card/selectable-role-card.component';
import { RequiredFormInputDirective } from 'src/app/shared/directives/required-form-input.directive';
import { TileSelectComponent } from 'src/app/shared/components/tile-select/tile-select.component';
import { MultiSelectComponent } from 'src/app/shared/components/multi-select/multi-select.component';
import { InfoCardsModule } from 'src/app/shared/components/info-cards/info-cards.module';
import { FillProfileRoutingModule } from './fill-profile-routing.module';
import { FillProfilePageComponent } from './fill-profile-page/fill-profile-page.component';
import { BorderCardComponent } from '../../shared/components/border-card/border-card.component';
import { StepperModule } from '../../shared/components/stepper/stepper.module';

@NgModule({
    declarations: [
        FillProfilePageComponent,
        FillProfileInfoComponent,
        WelcomeIntroComponent,
        FillProfileSkillsComponent,
        RolesSelectComponent,
        SelectableRoleCardComponent,
    ],
    imports: [
        CommonModule,
        FillProfileRoutingModule,
        NgOptimizedImage,
        BorderCardComponent,
        StepperModule,
        ReactiveFormsModule,
        NgIconsModule.withIcons({ bootstrapChatLeftHeartFill }),
        RequiredFormInputDirective,
        TileSelectComponent,
        MultiSelectComponent,
        InfoCardsModule,
    ],
})
export class FillProfileModule {}
