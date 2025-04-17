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
    this.subscription.add(
      this.authService.getCurrentUser().subscribe({
        next: (response) => {
          this.authService.currentUserSignal.set(response);
        },
        error: (error) => {
          console.log('Error fetching current user:', error);
        }
      })
    )
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
