import { AbstractControl, FormGroup } from '@angular/forms';

export class ValidatorField {

  static MustMatch(controlName: string, matchingControlName: string): any {
    return (group: AbstractControl) => {
      const formGroup = group as FormGroup;
      const control = formGroup.controls[controlName];
      const matchControl = formGroup.controls[matchingControlName];

      if (matchControl.errors && !matchControl.errors.mustMatch) {
        return null;
      }

      if (control.value !== matchControl?.value) {
        matchControl.setErrors({ mustMatch: true })
      } else {
        matchControl.setErrors(null);
      }

      return null;
    }
  }
}
