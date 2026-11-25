import { Component, inject, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../core/services/auth-service';
import { RegisterDto } from '../../models/auth/register-dto';
import { FormsModule } from '@angular/forms';

@Component({
  imports: [FormsModule,RouterLink],
  selector: 'app-register-component',
  styleUrl: './register-component.css',
  templateUrl: './register-component.html',
})
export class RegisterComponent {

  private authService = inject(AuthService);
  private router = inject(Router);

  errorMessage = signal<string | null>(null);
  
  registerDto: RegisterDto = {
  firstName: '',
  lastName: '',
  birthDate: '',
  email: '',
  password: ''
};

confirmPassword = '';

  register(): void {

      if (this.registerDto.password !== this.confirmPassword)
      {
          console.error('Passwords do not match.');
          return;
      }
      this.authService.register(this.registerDto).subscribe({
      next: () => 
      {
      this.router.navigate(['/home']);
      },
      error: (error) => 
      {
          this.errorMessage.set(
          error.error?.detail ?? 'An unexpected error occurred.'
          );
           console.log('Error message:', this.errorMessage());
      }
  });
}
}
