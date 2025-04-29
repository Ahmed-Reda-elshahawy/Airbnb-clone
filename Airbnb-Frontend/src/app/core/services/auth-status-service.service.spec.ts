import { TestBed } from '@angular/core/testing';

import { AuthStatusServiceService } from './auth-status-service.service';

describe('AuthStatusServiceService', () => {
  let service: AuthStatusServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AuthStatusServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
