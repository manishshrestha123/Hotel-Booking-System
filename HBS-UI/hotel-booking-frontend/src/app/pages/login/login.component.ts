import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ CommonModule,
    ReactiveFormsModule,
  RouterModule ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  loginForm: FormGroup;
  errorMessage: string | null = null;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });
  }

  isLoading = false;

  onSubmit(): void {
    if (this.loginForm.invalid) return;

    this.isLoading = true;
    this.errorMessage = null;

    // Simulate login call
    const val = this.loginForm.value;
    setTimeout(() => {
      this.isLoading = false;
      if (val.email.includes('admin')) {
        localStorage.setItem('access_token', 'fake-admin-jwt');
        localStorage.setItem('user_role', 'admin');
        this.router.navigate(['/admin/dashboard']);
      } else {
        localStorage.setItem('access_token', 'fake-guest-jwt');
        localStorage.setItem('user_role', 'guest');
        this.router.navigate(['/home']);
      }
    }, 1000);
  }
}