import { AbstractControl, ValidationErrors } from '@angular/forms';

export function emailOrPhoneValidator(
  control: AbstractControl
): ValidationErrors | null {
  const value = control.value;

  if (!value) return null;

  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  const phoneRegex = /^07[7-9]\d{7}$/;

  const isValidEmail = emailRegex.test(value);
  const isValidPhone = phoneRegex.test(value);

  return isValidEmail || isValidPhone ? null : { emailOrPhone: true };
}
