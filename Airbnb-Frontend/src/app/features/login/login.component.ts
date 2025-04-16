import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { ModalService } from '../../core/services/modal.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup = new FormGroup({});
  isLoading = false;
  isModalOpen = false;
  subscription: Subscription = new Subscription();
  // constructor(private fb: FormBuilder, private router: Router, private loginValidator: LoginEmailExistanceValidationService, private usersData: UsersDataService) {}
  constructor(private fb: FormBuilder, private router: Router, private modalService: ModalService) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.pattern('^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,}$')]],
    }, {
      asyncValidators: [
        // this.loginValidator.validateUserExists()
      ]
    });

    this.subscription = this.modalService.loginModal$.subscribe(isOpen => {
      this.isModalOpen = isOpen;
    });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  closeModal() {
    this.modalService.closeLoginModal();
  }

  closeModalOnBackdrop(event: MouseEvent) {
    if (event.target === event.currentTarget) {
      this.closeModal();
    }
  }

  onSubmit() {
    this.closeModal();
  }

  // onSubmit() {
  //   if (this.loginForm.invalid) {
  //     return;
  //   }

  //   this.isLoading = true;
  //   const { email, password } = this.loginForm.value;

  //   this.usersData.login(email, password).subscribe({
  //     next: (user: IUser) => {
  //       // Store user data in localStorage
  //       localStorage.setItem('user', JSON.stringify({
  //         email: user.email,
  //         role: user.role,
  //         id: user.id
  //       }));

  //       // Redirect based on role
  //       if (user.role === 'admin') {
  //         this.router.navigate(['/dashboard']);
  //       } else {
  //         this.router.navigate(['/products']);
  //       }
  //     },
  //     error: (err) => {
  //       this.isLoading = false;
  //       this.loginForm.setErrors({ invalidCredentials: true });
  //       console.error('Login error:', err);
  //     },
  //     complete: () => {
  //       this.isLoading = false;
  //     }
  //   });
  // }

}
