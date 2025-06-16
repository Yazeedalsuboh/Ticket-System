import { NgIf, NgSwitch, NgSwitchCase } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import {
  FormBuilder,
  NonNullableFormBuilder,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';
import { TranslatePipe } from '@ngx-translate/core';
import { ApiService } from '../../../shared/services/api.service';

@Component({
  selector: 'app-forgot-password',
  imports: [
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatButtonModule,
    TranslatePipe,
    ReactiveFormsModule,
    NgSwitch,
    NgSwitchCase,
    NgIf,
  ],
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.css',
})
export default class ForgotPasswordComponent {
  private readonly _formBuilder = inject(FormBuilder);
  private readonly _apiService = inject(ApiService);
  private readonly _router = inject(Router);

  nextStep = signal<string>('email');
  showPassword = false;

  togglePassword() {
    this.showPassword = !this.showPassword;
  }
  emailForm = this._formBuilder.group(
    {
      email: ['', [Validators.required]],
      code: [''],
      password: [
        '',
        [
          Validators.required,
          Validators.pattern(
            '(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[$@$!%*?&])[A-Za-zd$@$!%*?&].{8,}'
          ),
        ],
      ],
    },
    NonNullableFormBuilder
  );

  submitEmail() {
    this._apiService.forgotPassword(this.emailForm.value).subscribe((data) => {
      if (data.success) {
        this.nextStep.set('code');
      }
    });
  }

  submitCode() {
    this._apiService.verifyCode(this.emailForm.value).subscribe((data) => {
      if (data.success) {
        this.nextStep.set('reset');
      }
    });
  }

  submitNew() {
    this._apiService.updatePassword(this.emailForm.value).subscribe((data) => {
      if (data.success) {
        this._router.navigate(['auth/login']);
      }
    });
  }
}
