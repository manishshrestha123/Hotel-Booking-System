export interface Customer {
  id: string;
  fullName: string;
  email: string;
  phone: string;
}

export interface CreateCustomer {
  fullName: string;
  email: string;
  phone: string;
  // Included simply to collect from the guest UI as required
  dateOfBirth?: string; 
}
