import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../../services/auth.service';
import { CreateCustomerAccount, CustomerService } from '../../../services/customer.service';

@Component({
  selector: 'app-user-management',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './user-management.component.html',
  styleUrl: './user-management.component.scss'
})
export class UserManagementComponent {
  userForm: FormGroup;
  customerForm: FormGroup;
  currentRole: string;
  systemUserMessage = '';
  customerMessage = '';
  systemUserError = '';
  customerError = '';
  creatingUser = false;
  creatingCustomer = false;

  readonly systemRoles = [
    { label: 'Admin', value: 2 },
    { label: 'Staff', value: 3 }
  ];

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private customerService: CustomerService
  ) {
    this.currentRole = this.authService.getSession()?.role ?? '';

    this.userForm = this.fb.group({
      fullName: ['', Validators.required],
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      role: [2, Validators.required],
    });

    this.customerForm = this.fb.group({
      fullName: ['', Validators.required],
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required],
      dateOfBirth: [''],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  get canCreateSystemUsers(): boolean {
    return this.currentRole.toLowerCase() === 'superadmin';
  }

  createSystemUser(): void {
    if (!this.canCreateSystemUsers) {
      this.systemUserError = 'Only the superadmin can create admin or staff accounts.';
      return;
    }

    if (this.userForm.invalid) {
      this.userForm.markAllAsTouched();
      return;
    }

    this.creatingUser = true;
    this.systemUserError = '';
    this.systemUserMessage = '';

    this.authService.createUser(this.userForm.getRawValue()).subscribe({
      next: (response) => {
        this.creatingUser = false;
        this.systemUserMessage = response.message;
        this.userForm.reset({ role: 2 });
      },
      error: (err) => {
        this.creatingUser = false;
        this.systemUserError = err.error?.message || 'Could not create the system user.';
      }
    });
  }

  createCustomerAccount(): void {
    if (this.customerForm.invalid) {
      this.customerForm.markAllAsTouched();
      return;
    }

    this.creatingCustomer = true;
    this.customerError = '';
    this.customerMessage = '';

    const payload: CreateCustomerAccount = this.customerForm.getRawValue();
    this.customerService.createCustomerAccount(payload).subscribe({
      next: () => {
        this.creatingCustomer = false;
        this.customerMessage = 'Customer account created successfully.';
        this.customerForm.reset();
      },
      error: (err) => {
        this.creatingCustomer = false;
        this.customerError = err.error?.message || 'Could not create the customer account.';
      }
    });
  }
}
