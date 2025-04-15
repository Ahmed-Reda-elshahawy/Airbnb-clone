import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { NavigationService, NavItem } from '../../../core/services/navigation.service';

@Component({
  selector: 'app-airbnb-your-home-header',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, CommonModule],
  templateUrl: './airbnb-your-home-header.component.html',
  styleUrl: './airbnb-your-home-header.component.css'
})
export class AirbnbYourHomeHeaderComponent implements OnInit {
  isMobileMenuOpen = false;
  navItems: NavItem[] = [];

  constructor(private navigationService: NavigationService) {}

  ngOnInit() {
    this.navigationService.navItems$.subscribe(items => {
      this.navItems = items;
    });
  }

  toggleMobileMenu() {
    this.isMobileMenuOpen = !this.isMobileMenuOpen;
  }
}
