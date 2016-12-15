/******************************************************************************
* The MIT License
* Copyright (c) 2003 Novell Inc.  www.novell.com
* 
* Permission is hereby granted, free of charge, to any person obtaining  a copy
* of this software and associated documentation files (the Software), to deal
* in the Software without restriction, including  without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
* copies of the Software, and to  permit persons to whom the Software is 
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in 
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED AS IS, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*******************************************************************************/
//
// Novell.Directory.Ldap.Extensions.SplitPartitionRequest.cs
//
// Author:
//   Sunil Kumar (Sunilk@novell.com)
//
// (C) 2003 Novell, Inc (http://www.novell.com)
//

using System;
using System.IO;
using Novell.Directory.Ldap.Asn1;
using Novell.Directory.Ldap.Utilclass;

namespace Novell.Directory.Ldap.Extensions
{
    /// <summary>
    ///     Creates a new partition.
    ///     To split a new partition, you must create an instance of this
    ///     class and then call the extendedOperation method with this
    ///     object as the required LdapExtendedOperation parameter.
    ///     The SplitPartitionRequest extension uses the following OID:
    ///     2.16.840.1.113719.1.27.100.3
    ///     The requestValue has the following format:
    ///     requestValue ::=
    ///     flags  INTEGER
    ///     dn     LdapDN
    /// </summary>
    public class SplitPartitionRequest : LdapExtendedOperation
    {
        /// <summary>
        ///     Constructs an extended operation object for splitting partition.
        /// </summary>
        /// <param name="dn">
        ///     The distinguished name of the container where the new
        ///     partition  root should be located.
        /// </param>
        /// <param name="flags">
        ///     Specifies whether all servers in the replica ring must be up before
        ///     proceeding. When set to zero, the status of the servers is not
        ///     checked. When set to Ldap_ENSURE_SERVERS_UP, all servers must be up
        ///     for the operation to proceed.
        /// </param>
        /// <exception>
        ///     LdapException A general exception which includes an error message
        ///     and an Ldap error code.
        /// </exception>
        public SplitPartitionRequest(string dn, int flags) : base(ReplicationConstants.CREATE_NAMING_CONTEXT_REQ, null)
        {
            try
            {
                if ((object) dn == null)
                    throw new ArgumentException(ExceptionMessages.PARAM_ERROR);

                var encodedData = new MemoryStream();
                var encoder = new LBEREncoder();

                var asn1_flags = new Asn1Integer(flags);
                var asn1_dn = new Asn1OctetString(dn);

                asn1_flags.encode(encoder, encodedData);
                asn1_dn.encode(encoder, encodedData);

                setValue(SupportClass.ToSByteArray(encodedData.ToArray()));
            }
            catch (IOException ioe)
            {
                throw new LdapException(ExceptionMessages.ENCODING_ERROR, LdapException.ENCODING_ERROR, null);
            }
        }
    }
}