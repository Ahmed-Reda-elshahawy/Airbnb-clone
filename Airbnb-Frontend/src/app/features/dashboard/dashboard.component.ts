import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
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
import { DropdownModule } from 'primeng/dropdown';
import { SliderModule } from 'primeng/slider';
import { DashboardHeaderComponent } from "../dashboard-header/dashboard-header.component";
import { DashboardSideMenuComponent } from "../dashboard-side-menu/dashboard-side-menu.component";
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ConfirmationService } from 'primeng/api';
import { Subscription } from 'rxjs';

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
    DropdownModule,
    SliderModule,
    DashboardHeaderComponent,
    DashboardSideMenuComponent,
    ConfirmDialogModule
],
  providers: [ConfirmationService],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css',
})
export class DashboardComponent implements OnInit, OnDestroy {
  @ViewChild('sideMenu') sideMenu!: DashboardSideMenuComponent;
  users: User[] = [] as User[];
  representatives!: Representative[];
  statuses!: any[];
  loading!: boolean;
  activityValues: number[] = [0, 100];
  searchValue: string | undefined;
  subscription: Subscription = new Subscription();

  // Add properties for filter values
  selectedRepresentative: any;
  selectedStatus: any;
  selectedActivityRange: number[] = [0, 100];

  constructor(private userService: UserService, private confirmationService: ConfirmationService) {}

  ngOnInit() {
    // this.userService.getCustomersLarge().then((users) => {
    //   this.users = users;
    //   this.loading = false;

    //   this.users.forEach((user) => (user.date = new Date(<Date>user.date)));
    // });
    this.GetUsers();

    // this.representatives = [
    //   { name: 'Amy Elsner', image: 'amyelsner.png' },
    //   { name: 'Anna Fali', image: 'annafali.png' },
    //   { name: 'Asiya Javayant', image: 'asiyajavayant.png' },
    //   { name: 'Bernardo Dominic', image: 'bernardodominic.png' },
    //   { name: 'Elwin Sharvill', image: 'elwinsharvill.png' },
    //   { name: 'Ioni Bowcher', image: 'ionibowcher.png' },
    //   { name: 'Ivan Magalhaes', image: 'ivanmagalhaes.png' },
    //   { name: 'Onyama Limba', image: 'onyamalimba.png' },
    //   { name: 'Stephen Shaw', image: 'stephenshaw.png' },
    //   { name: 'Xuxue Feng', image: 'xuxuefeng.png' },
    // ];

    this.statuses = [
      { label: 'Host', value: 'Host' },
      { label: 'Guest', value: 'Guest' },
    ];
  }

  GetUsers(){
    this.loading = true;
    this.subscription = this.userService.getUsers().subscribe((data) => {
      this.users = data;
      console.log(data);
      this.loading = false;
    });
  }

  DeleteUser(id: string) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete this user?',
      header: 'Delete Confirmation',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.userService.deleteUser(id).subscribe((response) => {
          console.log(response);
          this.GetUsers();
        });
      }
    });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  clear(table: Table) {
    table.clear();
    this.searchValue = '';
  }

  getSeverity(status: string): 'success' | 'info' | undefined {
    switch (status) {
      case "Guest":
        return 'success';
      case "Host":
        return 'info';
      default:
        return undefined;
    }
  }

  toggleSideMenu() {
    if (this.sideMenu) {
      this.sideMenu.toggleMenu();
    }
  }
}
