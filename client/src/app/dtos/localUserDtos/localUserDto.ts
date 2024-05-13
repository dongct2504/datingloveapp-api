export interface LocalUserDto {
    localUserId: string;
    firstName: string | null;
    lastName: string | null;
    userName: string;
    email: string;
    isConfirmEmail: boolean;
    phoneNumber: string;
    isConfirmPhoneNumber: boolean;
    dateOfBirth: string | null;
    lockoutEndDate: string | null;
    lockoutEnable: boolean;
    role: string | null;
    imageUrl: string | null;
    address: string | null;
    ward: string | null;
    district: string | null;
    city: string | null;
}