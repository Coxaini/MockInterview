import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthPageComponent } from './auth-page/auth-page.component';
import { AuthRoutingModule } from './auth-routing.module';
import { NgIconsModule, provideNgIconsConfig } from '@ng-icons/core';
import { bootstrapGithub, bootstrapGoogle } from '@ng-icons/bootstrap-icons';
import { BorderCardComponent } from '@shared/components/border-card/border-card.component';
import { ReactiveFormsModule } from '@angular/forms';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';

@NgModule({
    declarations: [AuthPageComponent, RegisterComponent, LoginComponent],
    imports: [
        CommonModule,
        AuthRoutingModule,
        BorderCardComponent,
        NgIconsModule.withIcons({ bootstrapGithub, bootstrapGoogle }),
        ReactiveFormsModule,
    ],
    providers: [
        provideNgIconsConfig({
            size: '2rem',
        }),
    ],
})
export class AuthModule {}
