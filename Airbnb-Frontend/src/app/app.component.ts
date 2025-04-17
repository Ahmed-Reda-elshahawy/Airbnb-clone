import { Component, OnDestroy, OnInit } from '@angular/core';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { HeaderComponent } from "./shared/components/header/header.component";
import { FooterComponent } from "./shared/components/footer/footer.component";
import { LoginComponent } from "./features/login/login.component";
import { RegisterComponent } from "./features/register/register.component";
import { AuthService } from './core/services/auth.service';
import { Subscription } from 'rxjs';



@Component({
  selector: 'app-root',
  standalone:true,
  imports: [RouterOutlet, FooterComponent, HeaderComponent, LoginComponent, RegisterComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit, OnDestroy {
  title = 'Airbnb';
  showHeader: boolean = true;
  showFooter: boolean = true;
  userId!: string;
  firstName!: string;
  lastName!: string;
  role!: string;
  subscription: Subscription = new Subscription();
  constructor(private router: Router, private authService: AuthService){
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.showHeader = !event.url.includes('/dashboard') && !event.url.includes('/hosting');
        // this.showFooter = !event.url.includes('/dashboard') && !event.url.includes('/hosting');
      }
    });
  }

  ngOnInit(): void {
    console.log("data: ",this.authService.getAccessTokenData());
    this.subscription.add(
      this.authService.getCurrentUser().subscribe({
        next: (response) => {
          console.log('Current user:', response);
          this.authService.currentUserSignal.set(response);
          console.log("current user data signal: ",this.authService.currentUserSignal());
        },
        error: (error) => {
          console.error('Error fetching current user:', error);
        }
      })
    )
    // this.userId = this.authService.getAccessTokenClaim('http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier');
    // this.role = this.authService.getAccessTokenClaim('http://schemas.microsoft.com/ws/2008/06/identity/claims/role');
    // this.firstName = this.authService.getAccessTokenClaim('FirstName');
    // this.lastName = this.authService.getAccessTokenClaim('LastName');
    // console.log("userId: ",this.userId);
    // console.log("role: ",this.role);
    // console.log("firstName: ",this.firstName);
    // console.log("lastName: ",this.lastName);
    // console.log("exp: ", this.authService.getAccessTokenClaim('exp'))
    // this.firstName = this.authService.getTokenData().firstName;
    // this.lastName = this.authService.getTokenData().lastName;
    // this.role = this.authService.getTokenData().role;
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
