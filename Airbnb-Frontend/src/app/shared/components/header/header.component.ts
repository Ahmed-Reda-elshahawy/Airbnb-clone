import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-header',
  imports: [CommonModule, RouterModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  isUserMenuOpen = false;
  isLanguageMenuOpen = false;
  currentLanguage = 'English (US)';
  currencies = ['USD', 'EUR', 'GBP', 'AUD'];
  currentCurrency = 'USD';

  searchParams = {
    location: 'Anywhere',
    dates: 'Any week',
    guests: 'Add guests'
  };

  toggleUserMenu() {
    this.isUserMenuOpen = !this.isUserMenuOpen;
    // Close language menu if open
    if (this.isUserMenuOpen) {
      this.isLanguageMenuOpen = false;
    }
  }

  toggleLanguageMenu() {
    this.isLanguageMenuOpen = !this.isLanguageMenuOpen;
    // Close user menu if open
    if (this.isLanguageMenuOpen) {
      this.isUserMenuOpen = false;
    }
  }

  setLanguage(language: string) {
    this.currentLanguage = language;
    this.isLanguageMenuOpen = false;
  }

  setCurrency(currency: string) {
    this.currentCurrency = currency;
    this.isLanguageMenuOpen = false;
  }
}
