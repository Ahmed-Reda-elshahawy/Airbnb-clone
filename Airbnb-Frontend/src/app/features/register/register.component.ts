import { Component, OnInit } from '@angular/core';
import { RegisterModalService } from '../../core/services/register-modal.service';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { RouterModule } from '@angular/router';
import { FloatLabelModule } from 'primeng/floatlabel';
import { passwordMatchValidator } from '../../core/CrossFieldValidation/passwordMatchValidator';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, RouterModule, FormsModule, FloatLabelModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit {
  constructor(private registermodalService: RegisterModalService, private fb: FormBuilder) { }
  isModalOpen = false; // Track the modal state
  registerForm: FormGroup = new FormGroup({}); // Initialize the form group
  subscription: Subscription = new Subscription(); // Initialize the subscription
  isLoading = false; // Track loading state
  value: string = ''; // Initialize value for the input field

  ngOnInit(): void {
    this.subscription = this.registermodalService.registerModal$.subscribe(isOpen => {
      this.isModalOpen = isOpen;
    });
    this.registerForm = this.fb.group(
    {
      firstName: ['', [Validators.required, Validators.minLength(3)]],
      lastName: ['', [Validators.required, Validators.minLength(3)]],
      birthDate: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.pattern('^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,}$')]],
      confirmPassword: ['', [Validators.required]],
    },{
      validators: passwordMatchValidator()
    });
  }

  closeRegisterModal() {
    this.registermodalService.closeRegisterModal();
  }
  closeModalOnBackdrop(event: MouseEvent) {
    if (event.target === event.currentTarget) {
      this.closeRegisterModal();
    }
  }

  onSubmit() {
    this.closeRegisterModal();
  }

}
