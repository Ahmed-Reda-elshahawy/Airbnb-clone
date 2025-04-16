// import { Component } from '@angular/core';
// import { FormsModule } from '@angular/forms';
// import { TableModule } from 'primeng/table';

// @Component({
//   selector: 'app-dashboard',
//   imports: [
//     TableModule,
//     FormsModule
//   ],
//   templateUrl: './dashboard.component.html',
//   styleUrl: './dashboard.component.css'
// })
// export class DashboardComponent {
//   customers: any[] = [];
//   activityRange: number[] = [0, 100];
// }

import { Component, OnInit } from '@angular/core';
import { Table } from 'primeng/table';
import { TagModule } from 'primeng/tag';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { InputTextModule } from 'primeng/inputtext';
import { MultiSelectModule } from 'primeng/multiselect';
import { SelectModule } from 'primeng/select';
import { CommonModule } from '@angular/common';
import { TableModule } from 'primeng/table';
import { ProgressBar } from 'primeng/progressbar';
import { ButtonModule } from 'primeng/button';
import { Representative, User } from '../../core/models/user';
import { UserService } from '../../core/services/user.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-dashboard',
  imports: [
    TableModule,
    CommonModule,
    InputTextModule,
    TagModule,
    SelectModule,
    MultiSelectModule,
    ProgressBar,
    ButtonModule,
    IconFieldModule,
    InputIconModule,
    FormsModule,
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css',
})
export class DashboardComponent implements OnInit {
  users!: User[];
  representatives!: Representative[];
  statuses!: any[];
  loading: boolean = true;
  activityValues: number[] = [0, 100];
  searchValue: string | undefined;

  constructor(private userService: UserService) {}

  ngOnInit() {
    this.userService.getCustomersLarge().then((users) => {
      this.users = users;
      this.loading = false;

      this.users.forEach((user) => (user.date = new Date(<Date>user.date)));
    });

    this.representatives = [
      { name: 'Amy Elsner', image: 'amyelsner.png' },
      { name: 'Anna Fali', image: 'annafali.png' },
      { name: 'Asiya Javayant', image: 'asiyajavayant.png' },
      { name: 'Bernardo Dominic', image: 'bernardodominic.png' },
      { name: 'Elwin Sharvill', image: 'elwinsharvill.png' },
      { name: 'Ioni Bowcher', image: 'ionibowcher.png' },
      { name: 'Ivan Magalhaes', image: 'ivanmagalhaes.png' },
      { name: 'Onyama Limba', image: 'onyamalimba.png' },
      { name: 'Stephen Shaw', image: 'stephenshaw.png' },
      { name: 'Xuxue Feng', image: 'xuxuefeng.png' },
    ];

    this.statuses = [
      { label: 'Unqualified', value: 'unqualified' },
      { label: 'Qualified', value: 'qualified' },
      { label: 'New', value: 'new' },
      { label: 'Negotiation', value: 'negotiation' },
      { label: 'Renewal', value: 'renewal' },
      { label: 'Proposal', value: 'proposal' },
    ];
  }

  clear(table: Table) {
    table.clear();
    this.searchValue = '';
  }

  // getSeverity(status: string) {
  //   switch (status.toLowerCase()) {
  //     case 'unqualified':
  //       return 'danger';

  //     case 'qualified':
  //       return 'success';

  //     case 'new':
  //       return 'info';

  //     case 'negotiation':
  //       return 'warn';

  //     case 'renewal':
  //       return null;
  //   }
  // }
}
