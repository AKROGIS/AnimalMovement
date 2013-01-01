/* Simple Test Program for 6-bit CRC generation algorithm for T03 Format. */
#include <stdio.h>
#include <string.h>
#include <stdlib.h>

const char *byte_to_binary(int x)
{
    static char b[17];
    b[0] = '\0';

    int z;
    for (z = 32768; z > 0; z >>= 1)
    {
        strcat(b, ((x & z) == z) ? "1" : "0");
    }
    return b;
}


/****************************** Definitions ********************************/
typedef struct
{
unsigned long val; /* Field value */
int len; /* Field length in number of bits */
} field_T;
/***************************** Global Variables ****************************/
unsigned char crc;
/*************************** Function Prototypes ***************************/
void init_crc(void);
void crc_update(unsigned long inval, int nb);
int get_crc(void);
/******************************* Main Program ******************************/
main()
{
int     i;              /* Temp loop ctr */
int     a;              /* Returned CRC value */
/* char    binary_str[32]; */ /* Place to build strings for display */
char * binary_str;
field_T field[8] =      /* Field values to compute CRC of */
       {               /* Only 8 fields in this test case */
			/* Field      Field */
			/* Value      Length */ 
			 4L       ,   3,       /* These are the test case values */
			 10L      ,   4, 
			 30L      ,   5, 
			 55L      ,   6,
			 111L     ,   7,
			 222L     ,   8,
			 950L     ,   10,
			 3999L    ,   12
       };
/* Display test values */
printf("\n");
printf("Test case input values in decimal and binary:\n");
for ( i = 0; i < 8; i++ )
{
/* itoa( (int)field[i].val, binary_str, 2 ); */
binary_str = byte_to_binary((int)field[i].val);
printf( "%2d %4ld %*s\n", i+1, field[i].val, field[i].len, binary_str );
}

/* Calculate CRC value */
init_crc();
for ( i = 0; i < 8; i++ )
	crc_update(field[i].val, field[i].len );
	
/* Display CRC value */
a = get_crc();                     /* Get final CRC value */
/* itoa( a, binary_str, 2 );  */        /* Build a string of binary digits */
binary_str = byte_to_binary(a);
/* 
while ( strlen(binary_str) < 6 )   /* If string is less than 6 digits long... */ /*
{
strrev( binary_str ); 
strcat( binary_str, 2 ); 
strrev( binary_str );
}
*/
printf("\n6-bit CRC of all fields = %02X (hex) = %6s (binary)\n", a, binary_str);
printf("Done\n");
} /* End of main() */


/***************************************************************************
* These are the T03 Format 6-bit CRC routines
 **************************************************************************/
 
/***************************************************************************
 * init_crc --- initialize the crc value
  **************************************************************************/

void init_crc(void) {
	crc = 0x00;
} /* End of init_crc() */

/***************************************************************************
 * crc_update -- update the crc value with a new input value
  ***************************************************************************/
void crc_update(unsigned long inval, int nb)
/* Parameters: inval = Input value, right justified in long */
/*                nb = Number of significant bits in inval to include */ 
/*                     in CRC calculation from 1 to 32. */

{
#define FEEDBACK (unsigned char)0x43
#define MAXTAP (unsigned char)0x40

int i, /* Loop counter */
 bit1, /* Bit from CRC */
bit2; /* Bit from input value */

/* Left justify the input value */
for ( i=nb; i<32; i++ )
	inval <<= 1;

/* Update CRC */
for ( i=0; i<nb; i++, inval <<= 1 ) {
bit1 = (crc & MAXTAP) ? 1: 0; /* Get bit from CRC */
bit2 = (inval & 0x80000000L) ? 1: 0; /* Get bit from input value */
if ( bit1 ^ bit2 )
         crc ^= FEEDBACK;
crc <<= 1;
}
} /* End of crc_update() */

/***************************************************************************
 * get_crc -- returns the calculated CRC value, right justified and
 * limited to 6 bits.
 ***************************************************************************/
int get_crc(void) {
   crc >>= 1;   /* Right justify to eliminate extra zero bit */
   crc &= 0x3F; /* Limit to 6 bits */
   return crc;
} /* End of get_crc() */
