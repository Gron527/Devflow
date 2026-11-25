import { Component, inject, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../core/services/auth-service';
import { FormsModule } from '@angular/forms';
import { LoginDto } from '../../models/auth/login-dto';

@Component({
  imports: [FormsModule,RouterLink],
  selector: 'app-login-component',
  styleUrl: './login-component.css',
  templateUrl: './login-component.html',
})
export class LoginComponent {
  
  private authService = inject(AuthService);
  private router = inject(Router);
 
  errorMessage = signal<string | null>(null);
  loginDto: LoginDto = {
  email: '',
  password: ''
};
  
  login(): void {

  this.authService.login(this.loginDto).subscribe({
      next: () => 
      {
          this.router.navigate(['/home']);
      },
      error: (error) => 
      {
          this.errorMessage.set(
          error.error?.detail ?? 'An unexpected error occurred.'
          );
      }
  });
}
}
