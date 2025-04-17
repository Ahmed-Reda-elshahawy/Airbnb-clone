import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { ModalService } from '../../core/services/modal.service';
import { Subscription } from 'rxjs';
import { AuthService } from '../../core/services/auth.service';
import { ResponseUser } from '../../core/models/responseUser';


@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit, OnDestroy {
  loginForm: FormGroup = new FormGroup({});
  isLoading = false;
  isModalOpen = false;
  subscription: Subscription = new Subscription();
  currentUserDataFromToken: ResponseUser = {} as ResponseUser;
  // constructor(private fb: FormBuilder, private router: Router, private loginValidator: LoginEmailExistanceValidationService, private usersData: UsersDataService) {}
  constructor(private fb: FormBuilder, private router: Router, private modalService: ModalService, private authService: AuthService) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.pattern('^(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%^&*()_+\\-=\\[\\]{};\':"\\\\|,.<>\\/?]).{3,}$')]],
    }, {
      asyncValidators: [
        // this.loginValidator.validateUserExists()
      ]
    });

    this.subscription = this.modalService.loginModal$.subscribe(isOpen => {
      this.isModalOpen = isOpen;
    });
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
    this.isLoading = true;
    this.subscription.add(
      this.authService.login(this.loginForm.value).subscribe({
        next: (response) => {
          console.log(response);
          localStorage.setItem('accessToken', (response.accessToken));
          localStorage.setItem('refreshToken', (response.refreshToken));
          // const token = localStorage.getItem('accessToken');
          // if (token) {
          //   this.currentUserDataFromToken = jwt(token);
          // }
          // console.log("data: ",this.currentUserDataFromToken);
          // this.authService.currentUserSignal.set(this.currentUserDataFromToken);
          this.isLoading = false;
          this.closeModal();
          this.router.navigate(['/home']);
        },
        error: (error) => {
          this.isLoading = false;
          console.log(error);
        }
      })
    )
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
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
