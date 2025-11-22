import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Authservice } from '../../core/services/authservice';
import { Router } from '@angular/router';


@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  loginform!: FormGroup;
  isSubmitting = false;

  constructor(
    private fb: FormBuilder,
    private authservice: Authservice,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loginform = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
    });
  }

  onLogin(): void {
    if (this.loginform.invalid) {
      this.loginform.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;

    this.authservice.login(this.loginform.value).subscribe({
      next: (res) => {
        // ✅ Save all data (including userId)
        this.authservice.saveUserData(res.token, res.role, res.fullName, res.userId);

        alert('✅ Login successful!');

        // ✅ Redirect user based on role
        if (res.role === 'Admin') {
          this.router.navigate(['/admin']);
        } else {
          this.router.navigate(['/home']);
        }

        this.isSubmitting = false;
      },
      error: (err) => {
        console.error('Login failed:', err);
        alert('❌ Invalid email or password!');
        this.isSubmitting = false;
      }
    });
  }

}