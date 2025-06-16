import { Component, inject } from '@angular/core';
import { RegisterFormComponent } from '../../../shared/components/register-form/register-form.component';
import { Router } from '@angular/router';
import { ApiService } from '../../../shared/services/api.service';
import { register } from '../../../shared/types/auth-types';

@Component({
  selector: 'app-signup',
  imports: [RegisterFormComponent],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css',
})

// SignUp Controller
export default class SignupComponent {
  private readonly _apiService = inject(ApiService);
  private readonly _router = inject(Router);

  submit(formData: register) {
    this._apiService.signup(formData).subscribe(() => {
      this._router.navigate(['/auth/login']);
    });
  }
}
