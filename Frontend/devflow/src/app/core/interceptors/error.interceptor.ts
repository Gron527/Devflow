import { HttpInterceptorFn } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
    return next(req).pipe(
        catchError((error) => {
        if (error.status === 0) {
        // Hálózati hiba
        }
        else if (error.status >= 500) {
        // Szerverhiba
        }

  return throwError(() => error);
    })
  );
};