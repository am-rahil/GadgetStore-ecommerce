import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Authservice } from '../../core/services/authservice';

import { RegisterRequest } from '../../core/models/auth.models';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: false,
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {

  registerForm!: FormGroup;
  isSubmitting = false;

  constructor(
    private fb: FormBuilder,
    private authservice: Authservice,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.registerForm = this.fb.group({
      fullName: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      confirmPassword: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]],
      phoneNumber: ['', [Validators.required, Validators.pattern(/^[0-9]{10}$/)]]
    });
  }

  onRegister(): void {
    if (this.registerForm.invalid) return;

    const { password, confirmPassword } = this.registerForm.value;
    if (password !== confirmPassword) {
      alert("Passwords do not match!");
      return;
    }

    const request: RegisterRequest = {
      fullName: this.registerForm.value.fullName,
      email: this.registerForm.value.email,
      password: this.registerForm.value.password,
      phoneNumber: this.registerForm.value.phoneNumber
    };

    this.isSubmitting = true;

    this.authservice.register(request).subscribe({
      next: (res) => {
        alert('Registration successful! Please login.');
        this.router.navigate(['/auth/login']);
      },
      error: (err) => {
        console.error(err);
        alert('Registration failed. Try again.');
      },
      complete: () => (this.isSubmitting = false)
    });
  }

}
