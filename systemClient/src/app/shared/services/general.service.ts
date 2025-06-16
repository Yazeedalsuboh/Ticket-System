import { inject, Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root',
})
export class GeneralService {
  private readonly _snackBar = inject(MatSnackBar);

  public openSnackBar(message: string) {
    this._snackBar.open(message, 'Dismiss', { duration: 3000 });
  }
}
