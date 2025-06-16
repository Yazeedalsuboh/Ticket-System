import { Component, inject } from '@angular/core';
import { RegisterFormComponent } from '../../../shared/components/register-form/register-form.component';
import { ApiService } from '../../../shared/services/api.service';
import { Router } from '@angular/router';
import { register } from '../../../shared/types/auth-types';

@Component({
  selector: 'app-register-employee',
  imports: [RegisterFormComponent],
  templateUrl: './register-employee.component.html',
  styleUrl: './register-employee.component.css',
})
export default class RegisterEmployeeComponent {
  private readonly apiService = inject(ApiService);
  private readonly router = inject(Router);

  submit(formData: register) {
    this.apiService.registerEmployee(formData).subscribe(() => {
      this.router.navigate(['/employees/list']);
    });
  }
}
