export interface UpdateLocalUserDto {
    localUserId: string;
    firstName: string | null;
    lastName: string | null;
    dateOfBirth: string;
    userName: string;
    email: string;
    phoneNumber: string;
    gender: string;
    nickname: string;
    introduction: string | null;
    interest: string | null;
    idealType: string | null;
    address: string | null;
    ward: string | null;
    district: string | null;
    city: string | null;
    role: string | null;
}