import { HttpClient } from "@angular/common/http";
import { inject, Injectable, signal } from "@angular/core";
import { Observable, tap } from "rxjs";
import { RegisterDto } from "../../models/auth/register-dto";
import { AuthResponseDto } from "../../models/auth/authresponse-dto";
import { LoginDto } from "../../models/auth/login-dto";
import { environment } from "../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/auth`;

  private readonly tokenKey = 'auth_token';

  private readonly _isAuthenticated = signal<boolean>(
    !!localStorage.getItem(this.tokenKey)
  );

  readonly isAuthenticated = this._isAuthenticated.asReadonly();

  

  register(dto: RegisterDto): Observable<AuthResponseDto> {
    return this.http.post<AuthResponseDto>(
      `${this.apiUrl}/register`,
      dto
    ).pipe(
      tap(response => this.setToken(response.token))
    );
  }

  login(dto: LoginDto): Observable<AuthResponseDto> {
    return this.http.post<AuthResponseDto>(
      `${this.apiUrl}/login`,
      dto
    ).pipe(
      tap(response => this.setToken(response.token))
    );
  }



  logout(): void {
    localStorage.removeItem(this.tokenKey);
    this._isAuthenticated.set(false);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  private setToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
    this._isAuthenticated.set(true);
  }
}